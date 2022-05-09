using System.Collections.Generic;
public class Gate: GateInterface
{
    public Gate(int id, string name, string about, Dictionary<int, Pipe> inputs, 
            Dictionary<int, Source> outputs, Dictionary<int, GateInterface> inner, string type): 
            base(id, name, about, inputs, outputs, inner, type)
    {}

    public override void process()
    {
        if (_is_used == false)
        {
            _is_used = true;
            foreach (Source output in outputs.Values)
            {
                output.get_value();
            }
        }
    }

    public override void update()
    {
        _is_used = false;
        foreach (GateInterface item in _inner.Values)
        {   
            item.update();
        }
    }

    public override GateInterface copy()
    {
        Dictionary<int, Pipe> new_inputs = new Dictionary<int, Pipe>();
        foreach (int key in inputs.Keys)
        {
            new_inputs[key] = inputs[key].copy();
        }
        Dictionary<int, GateInterface> new_inner = new Dictionary<int, GateInterface>();
        foreach (int key in _inner.Keys)
        {
            new_inner[key] = _inner[key].copy();
        }

        foreach (int key in _inner.Keys)
        {
            foreach (int inp_key in new_inner[key].inputs.Keys)
            {
                PipeInterface connected = _inner[key].inputs[inp_key].get_connected_pipe();
                if (!(connected is null))
                {
                    if (!(connected.owner is null))
                    {
                        GateInterface new_owner = new_inner[connected.hyperowner.id];
                        new_inner[key].inputs[inp_key].set_value(new_owner.outputs[connected.id]);
                    }
                    else
                    {
                        new_inner[key].inputs[inp_key].set_value(new_inputs[connected.id]);
                    }
                    
                }
            }
  
        }

        Dictionary<int, Source> new_outputs = new Dictionary<int, Source>();
        foreach (int output_id in outputs.Keys)
        {
            GateInterface output_owner = outputs[output_id].owner;
            new_outputs[output_id] = new_inner[output_owner.id].outputs[output_id];
        }

        Gate copied_gate = new Gate(id: this.id, name: this.name, about: this.about, inputs: new_inputs,
             outputs: new_outputs, inner: new_inner, type: this.type);

        foreach (int out_id in copied_gate.outputs.Keys)
        {
            copied_gate.outputs[out_id].hyperowner = copied_gate;
        }

        return copied_gate;
    }

    public override int complexity()
    {
        return _inner.Count;
    }

    public override List<bool> check_state()
    {
        List<bool> res = new List<bool>();
        foreach (GateInterface elem in _inner.Values)
        {
            res.Add(elem.used());
        }
        return res;
    }

}