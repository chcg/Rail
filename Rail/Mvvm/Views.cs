using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Shell;

namespace Rail.Mvvm
{
    public static class Views
    {
        internal static void SetKeyBinding(this Window window, Key key, string commandName)
        {
            KeyBinding keyBinding = new KeyBinding() { Key = key };
            BindingOperations.SetBinding(keyBinding, KeyBinding.CommandProperty, new Binding(commandName));
            window.InputBindings.Add(keyBinding);
        }

        internal static void SetEventBinding(this Window window, string eventName, string commandName)
        {
            Microsoft.Xaml.Behaviors.EventTrigger trigger = new Microsoft.Xaml.Behaviors.EventTrigger(eventName);
            EventToCommand action = new EventToCommand();
            BindingOperations.SetBinding(action, EventToCommand.CommandProperty, new Binding(commandName));
            trigger.Actions.Add(action);
            Interaction.GetTriggers(window).Add(trigger);
        }
    }
}
