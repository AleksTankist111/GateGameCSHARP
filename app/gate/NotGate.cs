using System;

public class NotGate: GateInterface
{
    public NotGate(): this(1, "") 
    {}
    public NotGate(int id, string name): base(id: id, name: name, type: "NOT")
    {
        inputs[1] = new Pipe(id: 1, owner: this, name: "I1");

        set_new_output(2);
        price = new Price(and_gates: 0, not_gates: 1);

        //TODO: Вынести описания в отдельный файл?
        this.about = "This is the commonest gate in the world: NOT gate. \n It gives true input is false.";
    }
    public override void process()
    {   
        if (_is_used == false)
        {   
            _is_used = true;
            bool res = !inputs[1].get_value();

            outputs[2].set_value(res);
        }
    }

    public override GateInterface copy()
    {
        NotGate new_gate = new NotGate(id: id, name: name);
        return new_gate;
    }
}