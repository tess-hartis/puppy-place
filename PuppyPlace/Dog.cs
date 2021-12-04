namespace PuppyPlace;

public class Dog
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Breed { get; set; }
    public Person Owner { get; set; }
    public Dog(string name, int age, string breed)
    {
        Name = name;
        Age = age;
        Breed = breed;
    }
    public string Bark()
    {
        return "WOOF!!!";
    }
}