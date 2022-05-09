public abstract class PipeInterface
{
    public int id;
    public GateInterface owner;
    public GateInterface hyperowner;
    public string name;
    
    public PipeInterface(int id, GateInterface owner, string name)
    {        
        this.id = id;
        this.owner = owner;
        this.name = name;
    }

    // public virtual void set_value()
    //{}

    public abstract bool get_value();  


    public abstract PipeInterface copy();

    public abstract void delete_value();

}