#r "Microsoft.VisualBasic"
using Microsoft.VisualBasic;

string selectedIndex = Interaction.InputBox("Index for selected Item", "Index", "1", 740, 400);

int selectedIndexInt; 

bool isselectedIndexANumber = Int32.TryParse(selectedIndex, out selectedIndexInt); 

if (!isselectedIndexANumber) {
    Error(selectedIndex + " is not a positive integer. Invalid Index"); 
    return; 
}; 


string minIndex = Interaction.InputBox("Enter minimum Index", "Min Index", "2", 740, 400);

int minIndexInt; 

bool isminIndexANumber = Int32.TryParse(minIndex, out minIndexInt); 

if (!isminIndexANumber) {
    Error(minIndex + " is not an integer. Invalid index"); 
    return; 
}; 


string maxIndex = Interaction.InputBox("Enter maximum Index", "Min Index", "12", 740, 400);

int maxIndexInt; 

bool isMaxIndexANumber = Int32.TryParse(maxIndex, out maxIndexInt); 

if (!isMaxIndexANumber) {
    Error(maxIndex + " is not a positive integer. Invalid Index"); 
    return; 
} else if(maxIndexInt < selectedIndexInt) { 
    Error(maxIndex + " is smaller than " + selectedIndexInt +". Invalid Max Index."); 
    return; 
}; 


string indexStep = Interaction.InputBox("Enter step", "step", "1", 740, 400);

int indexStepInt; 

bool isindexStepANumber = Int32.TryParse(indexStep, out indexStepInt); 

if (!isindexStepANumber) {
    Error(indexStep + " is not a positive integer. Invalid Index"); 
    return; 
} else if(indexStepInt < 0) { 
    Error(indexStep + " is negative +". Invalid index step."); 
    return; 
}; 