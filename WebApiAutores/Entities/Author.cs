using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validations;

namespace WebApiAutores.Entities
{
    public class Author: IValidatableObject
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 120)]
        //[FirstCapitalLetter]
        public string Name { get; set; }

        //public int Minur { get; set; }
        //public int Major{ get; set; }


        public List<Book> Books { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(Name))
            {
                var firstLetter = Name[0].ToString();
                if(firstLetter != firstLetter.ToUpper())
                {
                    yield return new ValidationResult("The first letter must be capitalized",
                        new string[] { nameof(Name) });
                }
            }
            //if (Minur > Major)
            //{
            //    yield return new ValidationResult("This field must not be greater than major field",
            //        new string[] { nameof(Minur) });
            //}
        }
    }
}
