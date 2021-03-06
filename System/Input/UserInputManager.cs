﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SFML.Window;
using SFML.System;
using Andromeda.System.Utility;
using Andromeda.System.Input;

namespace Andromeda.System
{

    /// <summary>
    /// The input manager
    /// </summary>
    public partial class UserInputManager
    {
        Dictionary<string, Mouse.Button> buttonBindings = new Dictionary<string, Mouse.Button>();
        Dictionary<string, Keyboard.Key> keyBindings = new Dictionary<string, Keyboard.Key>();
        List<InputBindingAction> actions = new List<InputBindingAction>();
        List<InputBinding> bindings = new List<InputBinding>();

        ITextInput textInput;
        bool textInputFocused = false;
        string textInputString = "";

        public bool HasTextInputFocus
        {
            get => textInputFocused;
        }

        public string TextInputValue
        {
            get => textInput?.Value;
        }

        public ITextInput FocusedTextInput
        {
            get
            {
                return textInput;
            }
            set
            {
                if (value != null)
                {
                    textInputString = value.Value;
                    textInputFocused = true;
                    textInput = value;
                    textInput.Focused = true;
                }
                else
                {
                    if (textInput != null)
                        textInput.Focused = false;

                    textInputFocused = false;
                    textInput = null;
                }
                    
            }
        }

        /// <summary>
        /// Reset the input manager
        /// </summary>
        internal void ClearBindings()
        {
            bindings.Clear();
            actions.Clear();
            keyBindings.Clear();
            buttonBindings.Clear();
        }

        internal IOrderedEnumerable<InputBindingAction> ActionsByPriority
        {
            get => actions.OrderBy(e => e.BindingPriority);
        }

        /// <summary>
        /// Bind an action to a method with the specified inputs
        /// </summary>
        /// <param name="name">The name of this binding</param>
        /// <param name="actionMethod">The method that is called when these inputs are invoked</param>
        /// <param name="inputs">The inputs (Key, MouseButton, etc.)</param>
        public void BindAction(string name, Action<string, UserInputAction> actionMethod, params object[] inputs)
        {
            InputBindingAction action = new InputBindingAction(name, actionMethod, inputs);
            actions.Add(action);
            bindings.Add(action);
        }

        /// <summary>
        /// Rebinds an input action
        /// </summary>
        /// <param name="name">The name of the input action</param>
        /// <param name="inputs">The inputs</param>
        public void Rebind(string name, params object[] inputs)
        {
            var matchingBindings = bindings.Where(binding => binding.ActionName == name);
            foreach (var match in matchingBindings)
            {
                match.inputs = inputs;
            }
        }

        /// <summary>
        /// Rebinds an input action
        /// </summary>
        /// <param name="value">The input name</param>
        /// <param name="inputs">The inputs</param>
        public void Rebind(Enum value, params object[] inputs)
        {
            Rebind(value.GetFullName(), inputs);
        }


        /// <summary>
        /// Bind an action to a method with the specified inputs
        /// </summary>
        /// <param name="value">The enum value of this binding</param>
        /// <param name="actionMethod">The method that is called when these inputs are invoked</param>
        /// <param name="inputs">The inputs (Key, MouseButton, etc.)</param>
        public void BindAction(Enum value, Action<string, UserInputAction> actionMethod, params object[] inputs)
        {
            InputBindingAction action = new InputBindingAction(value.GetFullName(), actionMethod, inputs);
            actions.Add(action);
            bindings.Add(action);
        }

        enum CharacterType
        {
            Unknown,
            ASCII,
            Erase,
            Tab,
            Return,
            Done,
        }

        CharacterType GetCharacterType(char character)
        {
            char start = (char) 32; // space
            char end = (char)126; // tilde
            char delete = (char)127;
            char backspace = (char)8;
            char tab = (char)9;
            char esc = (char)27;
            char enter = (char)13;
           

            if ( character >= start && character <= end)
            {
                return CharacterType.ASCII;
            }
            else if (character == delete)
            {
                return CharacterType.Erase;
            }
            else if (character == backspace)
            {
                return CharacterType.Erase;
            }
            else if (character == tab)
            {
                return CharacterType.Tab;
            }
            else if (character == enter)
            {
                return CharacterType.Return;
            }
            else if (character == esc)
            {
                return CharacterType.Done;
            }
            else
            {
                return CharacterType.Unknown;
            }
        }

        internal void InvokeTextEntered(StateApplication stateApplication, string unicode)
        {
            if (FocusedTextInput != null)
            { 
                char code = char.Parse(unicode);
                switch (GetCharacterType(code))
                {
                    case CharacterType.ASCII:
                        textInputString += code;
                        break;
                    case CharacterType.Erase:
                        if (textInputString.Length > 0)
                            textInputString = textInputString.Remove(textInputString.Length - 1);
                        break;
                    case CharacterType.Tab:
                        textInputString += '\t';
                        break;
                    case CharacterType.Return:
                        if (textInput.IsMultiline)
                        {
                            textInputString += '\n';
                        }
                        else
                        {
                            textInput.Focused = false;
                            textInput = null;
                            textInputFocused = false;
                        }
                        break;
                    case CharacterType.Done:
                        textInput.Focused = false;
                        textInput = null;
                        textInputFocused = false;
                        break;
                }

                if (textInput != null)
                    textInput.Value = textInputString;
            }
        }

        /// <summary>
        /// Binds the inputs to an action name
        /// </summary>
        /// <param name="value">The enum value of the action</param>
        /// <param name="inputs">The inputs for this action</param>
        public void Bind(Enum value, params object[] inputs)
        {
            InputBinding binding = new InputBinding(value.GetFullName(), inputs);
            bindings.Add(binding);
        }

        /// <summary>
        /// Binds the inputs to an action name
        /// </summary>
        /// <param name="name">The name of the action</param>
        /// <param name="inputs">The inputs for this action</param>
        public void Bind(string name, params object[] inputs)
        {
            InputBinding binding = new InputBinding(name, inputs);
            bindings.Add(binding);
        }

        /// <summary>
        /// Returns whether or not the key is down
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool IsActive(string name)
        {
            var binding = bindings.Find(b => b.ActionName == name);
            if (binding != null)
            {
                return binding.HasKeyDown() || binding.HasMouseButtonPressed();
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Returns whether or not the key is down using an enum's name
        /// </summary>
        /// <param name="enumValue">The enum value</param>
        /// <returns></returns>
        public bool IsActive(Enum enumValue)
        {
            return IsActive(enumValue.GetFullName());
        }

        /// <summary>
        /// Invoke the input for the mouse wheel
        /// </summary>
        /// <param name="application">The application for the input</param>
        /// <param name="input">The input mouse button</param>
        /// <param name="delta">The delta movement of the scroll wheel</param>
        /// <param name="state">The state of the mouse button</param>
        public void InvokeInput(Application application, Mouse.Wheel input, Vector2f delta, InputState state)
        {
            
        }

        /// <summary>
        /// Invokes the input for the mouse
        /// </summary>
        /// <param name="application">The application for the input</param>
        /// <param name="input">The input mouse button</param>
        /// <param name="state">The state of the mouse button</param>
        public void InvokeInput(Application application, Mouse.Button input, InputState state)
        {
            foreach (InputBindingAction action in ActionsByPriority)
            {
                foreach (Mouse.Button button in action.MouseButtons)
                    if (button == input)
                        action.Action.Invoke(action.ActionName, new MouseInputAction(button, state, Mouse.GetPosition(application.Window)));
            }
        }

        /// <summary>
        /// Invokes input for the keyboard
        /// </summary>
        /// <param name="application">The application for the input</param>
        /// <param name="input">The input key</param>
        /// <param name="state">The state of the key</param>
        public void InvokeInput(Application application, Keyboard.Key input, InputState state)
        {
            if (!HasTextInputFocus)
            {
                foreach (InputBindingAction action in ActionsByPriority)
                {
                    foreach (KeyCombination combo in action.KeyCombinations)
                    {
                        if (combo.AllKeysPressed)
                        {
                            var keyInputAction = new KeyboardInputAction(state, combo.Keys);
                            action.Action.Invoke(action.ActionName, keyInputAction);
                        }
                    }

                    foreach (Keyboard.Key key in action.KeyCodes)
                        if (key == input)
                        {
                            var keyInputAction = new KeyboardInputAction(state, key);
                            action.Action.Invoke(action.ActionName, keyInputAction);
                        }


                    foreach (string stringKey in action.Strings)
                        if (stringKey == input.ToString())
                        {
                            var keyInputAction = new KeyboardInputAction(state, input);
                            action.Action.Invoke(action.ActionName, keyInputAction);
                        }
                }
            }
        }
    }
}
