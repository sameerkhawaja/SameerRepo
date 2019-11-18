//1
public interface IEmail{
    void SendEmail(string destination, string source, string bcc, string cc, string subject, string body);
}

//2
public class Email: IEmailSender{
    public void SendEmail(string recipient, string sender, string subject, string body){
        Console.WriteLine($"Recipient: {recipient}");
        Console.WriteLine($"Sender: {sender}");
        Console.WriteLine($"Subject: {subject}");
        Console.WriteLine($"Body: {body}");
    }
}

//3
// Formats required to support:
// First Last
// Last, First
public ParsedName NameParser(string fullname){
    if(!IsValidFormat){
        throw ArgumentException($"Name '{fullname}' is not in a supported format");
    }
    var parsedName = new ParsedName();
    if(fullname.Contains(',')){
        parsedName.LastName = fullname.Substring(0, fullname.IndexOf(','));
        parsedName.FirstName = fullname.Substring(fullname.IndexOf(' ')+1);
    }else{
        parsedName.FirstName = fullname.Substring(0, fullname.IndexOf(' '));
        parsedName.LastName = fullname.Substring(fullname.IndexOf(' ')+1);
    }
}

//this method would require unit testing to define names are considered valid formats
public bool IsValidFormat(string fullname){
    //one space, trailing spaces okay
    if(fullname.Trim().Count(Char.IsWhiteSpace) != 1){
        return false;
    }

    //one comma max
    if(fullname.Trim().Count(x => x == ',') > 1){
        return false;
    }

    //no special characters excluding ' or ,
    //D'Angelo Barksdale is considered a valid name
    var pattern = new Regex(@"[^(a-zA-Z', )]");
    var matches = pattern.Matches(fullname);
    if(matches.Count>0){
        return false;
    }
    return true;
}


//4
public class Service: INameParser, IEmailSender{
    private readonly INameParser nameParser;
    private readonly IEmailSender emailSender;

    public Service(){
        //register dependencies
    }

    public void SendBirthdayEmails(List<string> names){
        //parse name
        var parsedNames = new List<ParsedName>();
        foreach(string name in names){
            var parsedName = nameParser.ParseName(name);
            parsedNames.Add(parsedName);
        }
        //send email
        var from = "emaillist@fakecompany.com";
        var subject = "Happy Birthday";
        var body = "Happy Birthday to the following employees:\n";

        foreach(ParsedName parsedName in parsedNames){
            body = body + $"{parsedName.First} {parsedName.Last}\n";
        }

        foreach(ParsedName parsedName in parsedNames){
            var to = $"{parsedName.First}.{parsedName.Last}@fakecompany.com";
            emailSender.SendEmail(to, from, subject, body);
        }

    }



}