using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DndTable.Core.Actions;
using DndTable.Core.Characters;
using DndTable.Core.Dice;

namespace DndTable.Core.Log
{
    public class Logger : ILogger
    {
        private static Logger _singleton = new Logger();
        internal static Logger Singleton { get { return _singleton; } }


        private List<string> _messages = new List<string>();
        private const bool IgnoreZero = true;

        public List<string> GetAllMessages()
        {
            return _messages;
        }

        public List<string> GetLast(int count)
        {
            return _messages.Skip(Math.Max(0, _messages.Count() - count)).ToList();
        }

        public void Clear()
        {
            _messages.Clear();
        }


        internal void StartAction(int indent, BaseAction action)
        {
            _messages.Add(GetPrefix(indent) + action.Executer.CharacterSheet.Name + " starts action: " + action.Description);
        }

        internal void EndAction(int indent, BaseAction action)
        {
            _messages.Add(GetPrefix(indent) + action.Executer.CharacterSheet.Name + " end action: " + action.Description);
        }

        internal void StartCharacterSheetProperty(int indent, string property)
        {
            _messages.Add(GetPrefix(indent) + "Calculate: " + property);
        }

        internal void EndCharacterSheetProperty(int indent, string property)
        {
            _messages.Add(GetPrefix(indent) + "Calculation done: " + property);
        }

        internal void AddCharacterSheetProperty(int indent, string description, int value)
        {
            if (IgnoreZero && value != 0)
                _messages.Add(GetPrefix(indent) + "+" + value + ": " + description);
        }

        internal void LogAoO(int indent, ICharacter opportunist, ICharacter victim)
        {
            _messages.Add(GetPrefix(indent) + opportunist.CharacterSheet.Name + " has an AttackOfOpportunity against: " + victim.CharacterSheet.Name);
        }

        private static string GetPrefix(int indent)
        {
            //indent += 1;

            string prefix = string.Empty;
            for (var i=0; i < indent; i++)
            {
                prefix += "\t";
            }
            //for (var i=0; i < indent; i++)
            //{
            //    prefix += ">";
            //}

            if (indent > 0)
                prefix += " ";

            return prefix;
        }


        internal void AddRoll(DiceRoll roll)
        {
            _messages.Add("#" + roll.Description);
        }

    }
}
