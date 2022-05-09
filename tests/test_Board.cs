using System;
using System.Collections.Generic;

// TODO: доделать тесты
namespace test_Board
{
    class test
    {
        public static void get_test_or()
        {
            // Создаем доску с параметрами;
            
            Board board = new Board(n_sources: 2, n_sinks: 1, max_and_price: 10, max_not_price: 10);

            //Создаем гейты AND (1 штука):
            
            GateInterface and1 = new AndGate();

            //Создаем гейты NOT (3 штуки):
            
            GateInterface not1 = new NotGate(0, "not1");
            GateInterface not2 = new NotGate(0, "not2");
            GateInterface not3 = new NotGate(0, "not3");

            //Кладем их на поле:

            board.add_gate(not1);
            board.add_gate(not2);
            board.add_gate(not3);
            board.add_gate(and1);

            //Соединяем их так, чтобы получился "OR" гейт:

            and1.inputs[1].set_value(not1.outputs[2]);
            and1.inputs[2].set_value(not2.outputs[2]);
            not3.inputs[1].set_value(and1.outputs[3]);

            //Соединяем гейты с сурсом и стоком:

            not1.inputs[1].set_value(board.sources[1]);
            not2.inputs[1].set_value(board.sources[2]);
            board.sinks[3].set_value(not3.outputs[2]);

            //Проверяем результат:

            Dictionary<int, bool> res = board.result();
            Console.WriteLine($"Result with IN1={board.sources[1].get_value()} and IN2={board.sources[2].get_value()} is {res[3]}");

            //Изменяем начальные значения источников:
            board.sources[1].set_value(true);

            //Обновляем доску;

            board.step();

            //Проверяем результат снова:
            
            res = board.result();
            Console.WriteLine($"Result with IN1={board.sources[1].get_value()} and IN2={board.sources[2].get_value()} is {res[3]}");

            //Сохраняем гейт:

            Gate or_gate = board.create_gate(name: "OR1", type: "OR", about: "first OR gate");

            //Посмотрим на новосозданный гейт:

            Console.WriteLine($"Gate {or_gate.name}:");
            Console.WriteLine($"\t type: {or_gate.type};");
            Console.WriteLine($"\t about: {or_gate.about};");
            Console.WriteLine($"\t price: and={or_gate.price.and_price}, not={or_gate.price.not_price};");
            Console.WriteLine($"\t Inputs: {or_gate.inputs[1]}, {or_gate.inputs[2]};");
            Console.WriteLine($"\t Outputs: {or_gate.outputs[3]};");
            Console.WriteLine($"\t Complexity: {or_gate.complexity()};");
            
            //Очищаем доску:

            board.clear();

            //Кладем OR на доску и соединяем его с выходами и входами:

            board.add_gate(or_gate);
            or_gate.inputs[1].set_value(board.sources[1]);
            or_gate.inputs[2].set_value(board.sources[2]);
            board.sinks[3].set_value(or_gate.outputs[3]);

            //Обновляем доску и считаем результат:
            board.step();
            res = board.result();

            Console.WriteLine($"Result with IN1={board.sources[1].get_value()} and IN2={board.sources[2].get_value()} is {res[3]}");

            //Обновляем значения истоников и пересчитываем результат:

            board.sources[1].set_value(false);

            board.step();
            res = board.result();

            Console.WriteLine($"Result with IN1={board.sources[1].get_value()} and IN2={board.sources[2].get_value()} is {res[3]}");

        }

        public static void get_test_recursion()
        {
            // Замкнуть нот на себя и подключить его как вход в энд, второй конец энда соединить с инпутом.
            // Проверяется, работает ли такая конструкция "мигатель" как на поле, так и в качестве отдельного гейта.



            // Создаем доску с параметрами;
            
            Board board = new Board(n_sources: 2, n_sinks: 1, max_and_price: 10, max_not_price: 10);

            //Создаем гейты AND (1 штука):
            
            GateInterface and1 = new AndGate();

            //Создаем гейт NOT (1 штука):
            
            GateInterface not1 = new NotGate(0, "not1");

            //Кладем их на поле:

            board.add_gate(not1);
            board.add_gate(and1);

            // Соединяем между собой (и создаем замыкание на not)

            and1.inputs[1].set_value(not1.outputs[2]);
            not1.inputs[1].set_value(not1.outputs[2]);

            //Соединяем гейты с сурсом и стоком:

            and1.inputs[2].set_value(board.sources[2]);
            board.sinks[3].set_value(and1.outputs[3]);

            //Проверяем результат:

            Dictionary<int, bool> res = board.result();
            Console.WriteLine($"Result with IN2={board.sources[2].get_value()} is {res[3]}");

            // Делаем несколько итераций:
            
            board.step();
            res = board.result();
            Console.WriteLine($"Result with IN2={board.sources[2].get_value()} is {res[3]}");

            board.step();
            res = board.result();
            Console.WriteLine($"Result with IN2={board.sources[2].get_value()} is {res[3]}");

            board.step();
            res = board.result();
            Console.WriteLine($"Result with IN2={board.sources[2].get_value()} is {res[3]}");

            // Включаем источник и проверяем еще раз:

            board.sources[2].set_value(true);

            board.step();
            res = board.result();
            Console.WriteLine($"Result with IN2={board.sources[2].get_value()} is {res[3]}");

            board.step();
            res = board.result();
            Console.WriteLine($"Result with IN2={board.sources[2].get_value()} is {res[3]}");

            board.step();
            res = board.result();
            Console.WriteLine($"Result with IN2={board.sources[2].get_value()} is {res[3]}");
        
            // Сохраняем гейт:

            Gate new_gate = board.create_gate(name: "new_gate", about: "", type: "closure");
            board.clear();

            // Проверяем, удалились ли значения:
            res = board.result();
            Console.WriteLine($"Result with IN2={board.sources[2].get_value()} without gates on board is {res[3]}");

            // Кладем на доску созданный гейт и подключаем его:

            board.add_gate(new_gate);
            new_gate.inputs[2].set_value(board.sources[2]);

            board.sinks[3].set_value(new_gate.outputs[3]);

            // Проверяем еще раз:

            board.step();
            res = board.result();
            Console.WriteLine($"Result with IN2={board.sources[2].get_value()} is {res[3]}");

            board.step();
            res = board.result();
            Console.WriteLine($"Result with IN2={board.sources[2].get_value()} is {res[3]}");

            board.step();
            res = board.result();
            Console.WriteLine($"Result with IN2={board.sources[2].get_value()} is {res[3]}");

            // Выключаем источник и проверяем еще раз:

            board.sources[2].set_value(false);

            board.step();
            res = board.result();
            Console.WriteLine($"Result with IN2={board.sources[2].get_value()} is {res[3]}");

            board.step();
            res = board.result();
            Console.WriteLine($"Result with IN2={board.sources[2].get_value()} is {res[3]}");

            board.step();
            res = board.result();
            Console.WriteLine($"Result with IN2={board.sources[2].get_value()} is {res[3]}");

        }

        public static void get_test_unused()
        {
            // Проверить работу при наличии неподсоединенных гейтов\неподсоединенных входов\выходов.
            // Создать гейт с неподсоединенными гейтами/входами/выходами



            // Создаем доску с параметрами;
            
            Board board = new Board(n_sources: 2, n_sinks: 2, max_and_price: 10, max_not_price: 10);

            //Создаем гейты AND (1 штука):
            
            GateInterface and1 = new AndGate();

            //Создаем гейты NOT (1 штуки):
            
            GateInterface not1 = new NotGate(0, "not1");

            //Кладем их на поле:

            board.add_gate(not1);
            board.add_gate(and1);

            //Соединяем их:

            and1.inputs[1].set_value(not1.outputs[2]);

            // Включаем все инпуты:

            board.sources[1].set_value(true);
            board.sources[2].set_value(true);

            //Проверяем результат:

            Dictionary<int, bool> res = board.result();
            Console.WriteLine($"Result with IN1={board.sources[1].get_value()} and IN2={board.sources[2].get_value()} is {res[3]}");

            //Создадим новый гейт:

            Gate new_gate1 = board.create_gate(name: "not_used_gate", about:"", type: "no_used_gate");
            board.clear();
            Gate new_gate2 = board.create_gate(name: "empty_gate", about: "", type: "empty");

            //Выложим гейты на доску и подсоединим:

            board.add_gate(new_gate1);
            board.add_gate(new_gate2);

            board.sinks[3].set_value(new_gate1.outputs[3]);
            board.sinks[4].set_value(new_gate2.outputs[4]);

            // Получим ответ:

            res = board.result();
            Console.WriteLine($"Result is {res[3]}, {res[4]}");

            board.step();
            res = board.result();
            Console.WriteLine($"Result is {res[3]}, {res[4]}");
        }
    
        public static void get_test_deep_recursion()
        {
            // Проверить работу при создании гейта из других сложных гейтов

            // Создаем доску с параметрами;
            
            Board board = new Board(n_sources: 2, n_sinks: 1, max_and_price: 10, max_not_price: 10);

            //Создаем гейты AND (1 штука):
            
            GateInterface and1 = new AndGate();

            //Создаем гейты NOT (3 штуки):
            
            GateInterface not1 = new NotGate(0, "not1");
            GateInterface not2 = new NotGate(0, "not2");
            GateInterface not3 = new NotGate(0, "not3");

            //Кладем их на поле:

            board.add_gate(not1);
            board.add_gate(not2);
            board.add_gate(not3);
            board.add_gate(and1);

            //Соединяем их так, чтобы получился "OR" гейт:

            and1.inputs[1].set_value(not1.outputs[2]);
            and1.inputs[2].set_value(not2.outputs[2]);
            not3.inputs[1].set_value(and1.outputs[3]);

            //Соединяем гейты с сурсом и стоком:

            not1.inputs[1].set_value(board.sources[1]);
            not2.inputs[1].set_value(board.sources[2]);
            board.sinks[3].set_value(not3.outputs[2]);

            //Проверяем результат:

            Dictionary<int, bool> res = board.result();
            Console.WriteLine($"Result with IN1={board.sources[1].get_value()} and IN2={board.sources[2].get_value()} is {res[3]}");

            //Изменяем начальные значения источников:
            board.sources[1].set_value(true);

            //Обновляем доску;

            board.step();

            //Проверяем результат снова:
            
            res = board.result();
            Console.WriteLine($"Result with IN1={board.sources[1].get_value()} and IN2={board.sources[2].get_value()} is {res[3]}");

            //Сохраняем гейт:

            Gate or_gate = board.create_gate(name: "OR1", type: "OR", about: "first OR gate");

            //Посмотрим на новосозданный гейт:

            Console.WriteLine($"Gate {or_gate.name}:");
            Console.WriteLine($"\t type: {or_gate.type};");
            Console.WriteLine($"\t about: {or_gate.about};");
            Console.WriteLine($"\t price: and={or_gate.price.and_price}, not={or_gate.price.not_price};");
            Console.WriteLine($"\t Inputs: {or_gate.inputs[1]}, {or_gate.inputs[2]};");
            Console.WriteLine($"\t Outputs: {or_gate.outputs[3]};");
            Console.WriteLine($"\t Complexity: {or_gate.complexity()};");
            
            //Очищаем доску:

            board.clear();

            //Кладем OR на доску и соединяем его с выходами и входами:

            board.add_gate(or_gate);
            or_gate.inputs[1].set_value(board.sources[1]);
            or_gate.inputs[2].set_value(board.sources[2]);
            board.sinks[3].set_value(or_gate.outputs[3]);

            //Обновляем доску и считаем результат:
            board.step();
            res = board.result();

            Console.WriteLine($"Result with IN1={board.sources[1].get_value()} and IN2={board.sources[2].get_value()} is {res[3]}");

            // Добавляем еще один гейт на доску:

            GateInterface not4 = new NotGate(0, "not4");
            board.add_gate(not4);
            not4.inputs[1].set_value(board.sources[1]);
            or_gate.inputs[1].set_value(not4.outputs[2]);

            //Обновляем доску и считаем результат:
            
            board.step();
            res = board.result();

            Console.WriteLine($"Result with IN1={board.sources[1].get_value()} and IN2={board.sources[2].get_value()} is {res[3]}");

            //Создадим еще один гейт из того, что на доске:

            Gate new_gate = board.create_gate(name: "new_gate_rec", "", "recursive1");

            // Очистим доску и добавим новый гейт на нее:

            board.clear();
            board.add_gate(new_gate);

            //Подсоединим его к входам:
            new_gate.inputs[1].set_value(board.sources[1]);
            new_gate.inputs[2].set_value(board.sources[2]);
            board.sinks[3].set_value(new_gate.outputs[3]);

            //Обновляем доску и считаем результат:
            
            board.step();
            res = board.result();

            Console.WriteLine($"Result with IN1={board.sources[1].get_value()} and IN2={board.sources[2].get_value()} is {res[3]}");
            
            //Изменим входные условия, Обновляем доску и считаем результат:

            board.sources[1].set_value(false);
            board.step();
            res = board.result();

            Console.WriteLine($"Result with IN1={board.sources[1].get_value()} and IN2={board.sources[2].get_value()} is {res[3]}");
            

            // Очистим доску и добавим новый гейт на нее:

            Gate new_gate2 = board.create_gate(name: "new_gate_rec2", "", "recursive2");

            board.clear();
            board.add_gate(new_gate2);

            //Подсоединим его к входам:
            new_gate2.inputs[1].set_value(board.sources[1]);
            new_gate2.inputs[2].set_value(board.sources[2]);
            board.sinks[3].set_value(new_gate2.outputs[3]);

            //Обновляем доску и считаем результат:
            
            board.step();
            res = board.result();

            Console.WriteLine($"Result with IN1={board.sources[1].get_value()} and IN2={board.sources[2].get_value()} is {res[3]}");
            
            //Изменим входные условия, Обновляем доску и считаем результат:

            board.sources[1].set_value(true);
            board.step();
            res = board.result();

            Console.WriteLine($"Result with IN1={board.sources[1].get_value()} and IN2={board.sources[2].get_value()} is {res[3]}");
        

        
        }
    
        //Проверить delete функцию у Board

        // добавить проверку на прямые соединения (короткие замыкания)
    
    
    }
    
}