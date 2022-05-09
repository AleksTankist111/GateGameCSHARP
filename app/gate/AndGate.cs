using System;

public class AndGate: GateInterface
{
    public AndGate(): this(1, "") 
    {}
    public AndGate(int id, string name): base(id: id, name: name, type: "AND")
    {
        inputs[1] = new Pipe(id: 1, owner: this, name: "I1");
        inputs[2] = new Pipe(id: 2, owner: this, name: "I2");

        set_new_output(3);
        price = new Price(and_gates: 1, not_gates: 0);

        //TODO: Вынести описания в отдельный файл?
        this.about = "This is the commonest gate in the world: AND gate. \n It gives true when both inputs are true.";
    }
    public override void process()
    {   
        if (_is_used == false)
        {   
            _is_used = true;
            bool res = true;
            foreach (Pipe pipe in inputs.Values)
            {
                res = res && pipe.get_value();
                if (res == false)
                {
                    break;
                }
            }
            outputs[3].set_value(res);
        }
    }

    public override GateInterface copy()
    {
        AndGate new_gate = new AndGate(id: id, name: name);
        return new_gate;
    }
}