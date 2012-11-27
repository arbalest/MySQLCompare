using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace MySQLCompare.Utilities
{
    class ViewHelpers
    {
        /**
         * Utility for finding the visual parent of an element, of a certain type.
         * 
         * Useful for finding a specific parent object out of an event argument's OriginalSource property
         * 
         * Credit: Christian Mosers
         *         http://wpftutorial.net/DragAndDrop.html
         */
        public static T FindAncestor<T>(DependencyObject current)
            where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }
    }
}
