using System.ComponentModel.DataAnnotations;

namespace BDSA2017.Assignment06.Entities
{
    public class Car
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Driver { get; set; }
    }
}