using Models.HomeModels;

namespace Services.HomeServices
{
    public class HomeService : IHomeService
    {
        public Person _person ;
        public HomeService() {
            _person = new Person();
        }
        public void SetPerson(Person persona) {
            _person = persona;
        }

        public string Saludar() {
            return $"Hola {_person.Name} tienes {_person.getYearsOld()}";
        }
    }
}