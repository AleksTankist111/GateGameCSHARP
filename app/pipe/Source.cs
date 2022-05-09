using Adapters;

public class Source: PipeInterface
{
    
    private bool _value;

    public Source(int id, string name): this(id, null, name) 
    {}
    public Source(int id, GateInterface owner, string name): this(false, id, owner, name)
    {}
    public Source(bool value, int id, GateInterface owner, string name): base(id, owner, name)
    {
        this.set_value(value);
    }

    public void set_value(bool new_value)
    {
        this._value = new_value;
    }

    public override bool get_value()
    {
        //сравнить owner_id с null, если нет, то вызвать функцию хозяина
        if (!(owner is null))
            {owner.process();}
        return this._value;
        
    }

    public override Source copy()
    {
        Source new_instance = new Source(this.id, null, this.name);
        return new_instance;
    }

    public override void delete_value()
    {
        set_value(false);
    }

}