#r "Microsoft.VisualBasic"
using Microsoft.VisualBasic;


if(Selected.Measures.Count != 1) {
    Error("Select one and only one measure"); 
    return; 
} 

var selectedMeasure = Selected.Measure;

string newMeasureName = Interaction.InputBox("New Measure name", "Name", selectedMeasure.Name + " modified", 740, 400);



