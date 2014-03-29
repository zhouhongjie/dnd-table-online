using System;
using DndTable.Core.Actions;
using DndTable.Core.Log;

namespace DndTable.Core.Characters
{
    static class Calculator
    {
        private static int _indentLevel = 0;

        public static CalculatorPropertyContext CreatePropertyContext(string description)
        {
            return new CalculatorPropertyContext(description);
        }

        public static CalculatorActionContext CreateActionContext(BaseAction action)
        {
            return new CalculatorActionContext(action);
        }

        public class CalculatorPropertyContext : IDisposable
        {
            private string _contextDescription;
            private int _localIndentLevel;

            internal CalculatorPropertyContext(string contextDescription)
            {
                _localIndentLevel = _indentLevel++;
                Logger.Singleton.StartCharacterSheetProperty(_localIndentLevel++, contextDescription);
                _contextDescription = contextDescription;
            }

            public int Use(int value, string description)
            {
                Logger.Singleton.AddCharacterSheetProperty(_localIndentLevel, description, value);
                return value;
            }

            public void Dispose()
            {
                //Logger.Singleton.EndCharacterSheetProperty(_localIndentLevel, _contextDescription);
                _indentLevel--;
            }
        }

        public class CalculatorActionContext : IDisposable
        {
            private BaseAction _action;
            private int _localIndentLevel;

            internal CalculatorActionContext(BaseAction action)
            {
                _localIndentLevel = _indentLevel++;
                Logger.Singleton.StartAction(_localIndentLevel++, action);
               _action = action;
            }

            //public int Use(int value, string description)
            //{
            //    Logger.Singleton.AddCharacterSheetProperty(description, value);
            //    return value;
            //}

            public void Dispose()
            {
                //Logger.Singleton.EndAction(_localIndentLevel, _action);
                _indentLevel--;
            }

            public void AoO(ICharacter opportunist, ICharacter victim)
            {
                Logger.Singleton.LogAoO(_localIndentLevel, opportunist, victim);
            }
        }

    }
}