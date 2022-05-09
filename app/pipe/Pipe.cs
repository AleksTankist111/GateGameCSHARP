using Adapters;

public class Pipe: PipeInterface
{
    private PipeInterface _value;
    public Pipe(int id, string name): this(id, null, name)
    {}
    public Pipe(int id, GateInterface owner, string name): this(null, id, owner, name)
    {}
    public Pipe(PipeInterface value, int id, GateInterface owner, string name): base(id, owner, name)
    {
        this._value = value;
    }

    public void set_value(PipeInterface new_value)
    {
        this._value = new_value;
    }

    public override bool get_value()
    {   
        if (!(this._value is null))
        {
            return this._value.get_value();
        }
        else
        {
            return false;
        }
    }

    public PipeInterface get_connected_pipe()
    {
        return _value;
    }

    public override Pipe copy()
    {
        Pipe new_instance = new Pipe(this.id, null, this.name);
        return new_instance;
    }

    public override void delete_value()
    {
        set_value(null);
    }

}