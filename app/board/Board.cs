using System.Collections.Generic;
using Adapters;

public class Board
{   
    public Dictionary<int, Source> sources;
    public Dictionary<int, Pipe> sinks;
    public Dictionary<int, GateInterface> gates;
    public Price current_price;
    public Price max_price;
    private int available_id;

    public Board(int n_sources, int n_sinks, int max_and_price, int max_not_price)
    {
        available_id = 1;
        current_price = new Price();
        max_price = new Price(and_gates: max_and_price, not_gates: max_not_price);

        sources = new Dictionary<int, Source>();
        for (int i=1; i<=n_sources; i++)
        {
            sources[available_id] = new Source(id: available_id, name: $"IN{available_id}");
            available_id += 1;
        }

        sinks = new Dictionary<int, Pipe>();
        for (int i=1; i<=n_sinks; i++)
        {
            sinks[available_id] = new Pipe(id: available_id, name: $"OUT{available_id}");
            available_id += 1;
        }

        gates = new Dictionary<int, GateInterface>();
    }

    public Board(Dictionary<int, Source> sources, Dictionary<int, Pipe> sinks, Price max_price)
    {
        this.max_price = max_price;
        available_id = 1;
        current_price = new Price();

        this.sources = sources;
        this.sinks = sinks;

        available_id += sources.Count + sinks.Count;

        gates = new Dictionary<int, GateInterface>();

        //Возможно вставить проверку на отсутствие в сурсах и синках элементов с id>=available_id
    }

    public void add_gate(GateInterface gate)
    {   
        if (gate.price + current_price <= max_price)
        {
            gate.id = available_id;
            gates.Add(gate.id, gate);
            available_id += 1;
            current_price += gate.price;
        }
    }

    public void delete_gate(int id)
    {
        current_price -= gates[id].price;
        gates.Remove(id);
    }

    public void clear()
    {
        gates.Clear();
        available_id = sources.Count + sinks.Count + 1;
        current_price -= current_price;
        foreach (Pipe sink in sinks.Values)
        {
            sink.delete_value();
        }
    }

    public void step()
    {
        foreach (GateInterface gate in gates.Values)
        {   
            gate.update();
        }
    }

    public Dictionary<int, bool> result()
    {
        Dictionary<int, bool> res = new Dictionary<int, bool>();
        foreach (Pipe sink in sinks.Values)
        {   
            res[sink.id] = sink.get_value();
        }
        return res;
    }

    public Dictionary<int, bool>[] results_sequence(int n_steps)
    {
        Dictionary<int, bool>[] res = new Dictionary<int, bool>[n_steps];
        for (int i=1; i<=n_steps; i++)
        {
            res[i] = result();
            step();
        }
        return res;
    }

    public Gate create_gate(string name, string about, string type)
    {
        Dictionary<int, Pipe> new_inputs = new Dictionary<int, Pipe>();
        foreach (int key in sources.Keys)
        {   
            new_inputs[key] = new Pipe(sources[key].id, sources[key].name); 
        }
        Dictionary<int, GateInterface> new_inner = new Dictionary<int, GateInterface>();
        foreach (int key in gates.Keys)
        {
            new_inner[key] = gates[key].copy();
        }

        foreach (int key in gates.Keys)
        {
            foreach (int inp_key in new_inner[key].inputs.Keys)
            {
                PipeInterface connected = gates[key].inputs[inp_key].get_connected_pipe();
                if (!(connected is null))
                {
                    if (!(connected.owner is null))
                    {
                        GateInterface new_owner = new_inner[connected.owner.id];
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
        foreach (int output_id in sinks.Keys)
        {   
            IsGate new_sink = new IsGate(id: output_id, name: $"IS{output_id}");
           

            PipeInterface old_sink_connector = sinks[output_id].get_connected_pipe();
            if (!(old_sink_connector is null))
            {
                if (!(old_sink_connector.owner is null))
                {   

                    GateInterface new_sink_connector = new_inner[old_sink_connector.hyperowner.id];
                    new_sink.inputs[1].set_value(new_sink_connector.outputs[old_sink_connector.id]);
                    

                }
                else
                {
                    new_sink.inputs[1].set_value(new_inputs[old_sink_connector.id]);
                }
            }
            
            
            new_inner[output_id] = new_sink;

            new_outputs[output_id] = new_sink.outputs[output_id];
        }

        Gate copied_gate = new Gate(id: 1, name: name, about: about, inputs: new_inputs,
             outputs: new_outputs, inner: new_inner, type: type);
            
        return copied_gate;
    }
}