
namespace Adapters
{
    public class IsGate: GateInterface
    {
        public IsGate(): this(1, "") 
        {}
        public IsGate(int id, string name): base(id: id, name: name, type: "IS")
        {
            inputs[1] = new Pipe(id: 1, owner: this, name: "I1");

            set_new_output(id);
            price = new Price(and_gates: 0, not_gates: 0);

            //TODO: Вынести описания в отдельный файл?
            this.about = "This is Adapter for creating complex Gates called IS gate.";
        }
        public override void process()
        {   
            if (_is_used == false)
            {   
                _is_used = true;
                bool res = inputs[1].get_value();

                foreach (int key in outputs.Keys)
                {
                    outputs[key].set_value(res);
                }
                
            }
        }

        public override GateInterface copy()
        {
            IsGate new_gate = new IsGate(id: id, name: name);
            return new_gate;
        } 

        public override int complexity()
        {
            return 0;
        }
    
    }

   
}
