namespace Ezley.ValueObjects
{
    public class Editable<T>
    {
        public bool Edit { get;  set; } = false;
  
        public T Value { get;  set; }

        /// <summary>
        /// Edit is set to true by default if you use this constructor.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="edit"></param>
        public Editable(T value, bool edit = true)
        {
            Edit = edit;
            Value = value;
        }

        /// <summary>
        /// Edit is set to false by default with this constructor.
        /// </summary>
        public Editable()
        {
            Edit = false;
        }
    }
}