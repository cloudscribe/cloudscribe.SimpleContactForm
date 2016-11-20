using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContactForm.Models
{
    public class MessageResult
    {
        private static readonly MessageResult _success = new MessageResult { Succeeded = true };
        private List<MessageError> _errors = new List<MessageError>();

        /// <summary>
        /// Flag indicating whether if the operation succeeded or not.
        /// </summary>
        /// <value>True if the operation succeeded, otherwise false.</value>
        public bool Succeeded { get; protected set; }

        /// <summary>
        /// An <see cref="IEnumerable{T}"/> of <see cref="MessageError"/>s containing an errors
        /// that occurred during the identity operation.
        /// </summary>
        /// <value>An <see cref="IEnumerable{T}"/> of <see cref="MessageError"/>s.</value>
        public IEnumerable<MessageError> Errors => _errors;

        /// <summary>
        /// Returns an <see cref="MessageResult"/> indicating a successful operation.
        /// </summary>
        /// <returns>An <see cref="MessageResult"/> indicating a successful operation.</returns>
        public static MessageResult Success => _success;

        /// <summary>
        /// Creates an <see cref="MessageResult"/> indicating a failed operation, with a list of <paramref name="errors"/> if applicable.
        /// </summary>
        /// <param name="errors">An optional array of <see cref="MessageError"/>s which caused the operation to fail.</param>
        /// <returns>An <see cref="MessageResult"/> indicating a failed operation, with a list of <paramref name="errors"/> if applicable.</returns>
        public static MessageResult Failed(params MessageError[] errors)
        {
            var result = new MessageResult { Succeeded = false };
            if (errors != null)
            {
                result._errors.AddRange(errors);
            }
            return result;
        }

        /// <summary>
        /// Converts the value of the current <see cref="MessageResult"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of the current <see cref="MessageResult"/> object.</returns>
        /// <remarks>
        /// If the operation was successful the ToString() will return "Succeeded" otherwise it returned 
        /// "Failed : " followed by a comma delimited list of error codes from its <see cref="Errors"/> collection, if any.
        /// </remarks>
        public override string ToString()
        {
            return Succeeded ?
                   "Succeeded" :
                   string.Format("{0} : {1}", "Failed", string.Join(",", Errors.Select(x => x.Code).ToList()));
        }
    }
}
