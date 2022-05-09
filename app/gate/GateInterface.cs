using System.Collections.Generic;
using System;


public abstract class GateInterface
{
    public Dictionary<int, Pipe> inputs;
    public Dictionary<int, Source> outputs;
    public Price price;
    public string name;
    public string about;
    public readonly string type;

    private protected int _id;
    public int id
    {
        get {return _id;}
        set {_id = value;}
    }

    private protected bool _is_used;
    private protected Dictionary<int, GateInterface> _inner;


    public GateInterface(int id, string name, string type): this(id, name, "", type)
    {}
    public GateInterface(int id, string name, string about, string type): this(id, name, about, 
        new Dictionary<int, Pipe>(), new Dictionary<int, Source>(), 
        new Dictionary<int, GateInterface>(), type)
    {}

    public GateInterface(int id, string name, string about, Dictionary<int, Pipe> inputs, 
        Dictionary<int, Source> outputs, Dictionary<int, GateInterface> inner, string type)
    {   
        this.inputs = inputs;
        this.outputs = outputs;
        this._inner = inner;

        this.type = type;
        this.name = name;
        this.about = about;
        this.id = id;
        this._is_used = false;

        this.price = new Price();
        foreach (GateInterface gate in this._inner.Values)
        {   
            this.price += gate.price;
        }
        foreach (Source output in this.outputs.Values)
        {
            output.hyperowner = this;
        }

        //Добавить соединение иннеров с инпутами и аутпутами?
    }
    
    public virtual void update()
    {
        this._is_used = false;
    }

    public Dictionary<int, bool> get_results()
    {   

        process();
        Dictionary<int, bool> last_outputs = new Dictionary<int, bool>();
        foreach (int _id in outputs.Keys)
        {
            last_outputs[_id] = outputs[_id].get_value();
        }
        return last_outputs;
    }

    public abstract void process();

    public abstract GateInterface copy();
    
    public bool used() //Удалить после дебага
    {
        return _is_used;
    }

    public virtual int complexity()
    {
        return 1;
    }

    public virtual List<bool> check_state()
    {return null;}

    protected void set_new_output(int id)
    {
        outputs[id] = new Source(id: id, owner: this, name: "O1");
        outputs[id].hyperowner = this;
    }
}