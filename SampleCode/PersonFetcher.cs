// (C#) Write the worker class for a webservice to support a high volume of requests that filter a very large list of Person entities from a data source.  Include the ability for fuzzy/partial matching by first name, last name, or age (e.g. all Persons with last name starting with "s" who are over 30).

// Please define an abstracted data source, but don't worry about the underlying technology.  Your worker should be able to output either JSON or XML (rather than relying on a webservice framework).  Consider also testability.

// Reference code:

// 	public class Person
// 	{
// 		/// <summary>Primary key</summary>
// 		public int Id { get; set; }
// 		/// <summary>Given name</summary>
// 		public string FirstName { get; set; }
// 		/// <summary>Surname</summary>
// 		public string LastName { get; set; }
// 		/// <summary>Age in Years</summary>
// 		public float Age { get; set; }
// 	}


public interface IPersonFetcher{
    
    List<Person> GetAllPeople();
    List<Person> GetPeopleByFirstName(string firstName);
    List<Person> GetPeopleByLastName(string lastName);
    List<Person> GetPeopleByAge(float age);
    List<Person> GetPeopleByCriteria(string firstName, string lastName, float age);
}

//datasource is another service that returns all the people as a List<Person>
public class PersonFetcher: IPersonFetcher{
    
    public List<Person> GetAllPeople(){
        var personService = new PersonService();
        return personService.GetAllPeople();
    }

    public List<Person> GetPeopleByFirstName(string firstName){
        var personService = new PersonService();
        var people = personService.GetAllPeople();
        return people.Where(x => string.Equals(x.FirstName == firstName, StringComparison.OrdinalIgnoreCase));
    }
    public List<Person> GetPeopleByLastName(string lastName){
        var personService = new PersonService();
        var people = personService.GetAllPeople();
        return people.Where(x => string.Equals(x.LastName, lastName, StringComparison.OrdinalIgnoreCase));
    }
    public List<Person> GetPeopleByAge(float age){
        var personService = new PersonService();
        var people = personService.GetAllPeople();
        return people.Where(x => x.Age == age);
    }

    public List<Person> GetPeopleByCriteria(string firstNameCriteria, string lastNameCriteria, float ageCriteria, bool greaterThan){
        var personService = new PersonService();
        var people = personService.GetAllPeople();
        var personFilterByCriteria = new PersonFilterByCriteria();

        if(firstNameCriteria.length == 1){
            people = personFilterByCriteria.FirstNameStartsWith(firstNameCriteria, person);
        }else if(firstNameCriteria.length > 1){
            people = personFilterByCriteria.FirstNameContains(firstNameCriteria, person);
        }

        if(lastNameCriteria.length == 1){
            people = personFilterByCriteria.LastNameStartsWith(lastNameCriteria, person);
        }else if(lastNameCriteria.length > 1){
            people = personFilterByCriteria.LastNameContains(lastNameCriteria, person);
        }

        if(ageCriteria != 0 && greaterThan){
            people = personFilterByCriteria.AgeGreaterThanInclusive(ageCriteria, person);
        }else if(ageCriteria != 0 && !greaterThan){
            people = personFilterByCriteria.AgeLessThan(ageCriteria, person);
        }

        return people;
    }
}

public class PersonFilterByCriteria{
    public List<Person> FirstNameStartsWith(List<Person> people, string c){
        return people.Where(x => x.FirstName.ToLower().StartsWith(c));
    }

    public List<Person> LastNameStartsWith(string c, List<Person> people){
        return people.Where(x => x.LastName.ToLower().StartsWith(c));
    }

    public List<Person> FirstNameContains(string str, List<Person> people){
        return people.Where(x => x.FirstName.ToLower().Contains(str));
    }

    public List<Person> LastNameContains(string str, List<Person> people){
        return people.Where(x => x.LastName.ToLower().Contains(str));
    }

    public List<Person> AgeGreaterThanInclusive(float age, List<Person> people){
        return people.Where(x => x.Age>=age);
    }
    public List<Person> AgeLessThan(float age, List<Person> people){
        return people.Where(x => x.Age<age);
    }
}


//other notes
//I would use SimpleInjector to register PersonService so that I don't have to declare
//var personService = new PersonService();
//in every method
//I would include the following to pull out registered dependency
//public readonly PersonService personService;
  //  
    //public PersonFetcher(
     //   IPersonService personService
      //  ){
    //    this.personService = personService;
    //}