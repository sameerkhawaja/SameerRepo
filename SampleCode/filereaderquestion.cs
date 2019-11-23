public class StudentDataCopier{
    
    int idIndex;
    int nameIndex;
    int gradeIndex;

    public void ReadFromFile(){
        var input = @"C:\students.csv";
        var output = @"C:\output.csv";

        using(var reader = new StreamReader(input))
        {
            using(var writer = new StreamWriter(output))
            {
                //copy header
                var headerValues = GetValuesFromLine(reader);
                AssignHeaderIndex(headerValues);
                CopyLine(headerValues, writer);

                //copy rest of file
                while (!reader.EndOfStream)
                {
                    var values = GetValuesFromLine(reader);
                    var student = new Student{
                        name = values[nameIndex],
                        id = values[idIndex],
                        grade = values[gradeIndex]
                    };
                    CopyLine(values, writer);                   
                }
            }
        }
    }

    private string[] GetValuesFromLine(StreamReader reader){
        var line = reader.ReadLine();
        var values = line.Split('\t');
        return values;
    }

    private void CopyLine(string[] values, StreamWriter writer){
        writer.WriteLine(String.Join(" ", values));
    }

    private void AssignHeaderIndex(string[] headerValues){
        for(int i = 0; i<headerValues.length; i++)
            if(string.Equals(headerValues[i].ToLower(), "id")){
                idIndex = i;
            } else if(string.Equals(headerValues[i].ToLower(), "name")){
                nameIndex = i;
            } else if (string.Equals(headerValues[i].ToLower(), "grade")){
                gradeIndex = i;
        }
    }
}

//sample csv file with header
//Id  name grade
//001 cc  85
//002 bb  85
//003 aa  85


class Student{
    string name {get; set;}
    long id {get; set;}
    int grade {get; set;}
}
