import sys
import os
import argparse
from tabulate import tabulate

#Defines which can change per language
singleLineComment = "//"
multiLineCommentStart = "/*"
multiLineCommentEnd = "*/"
preProcessorDirectives = "#"

#This function opens up a single file and will do the break down
def parseFile(name):
    file = open(name, "r")
    #output = open("output.txt", "w")

    #flag for if in a multi line comment
    skippingLines = False

    #return dictionary
    counts = {
    "preProcessor" : 0,
    "multiLine" : 0,
    "singleLine" : 0,
    "whiteSpace" : 0,
    "code" : 0,
    }

    #For every line in the file
    for rawLine in file:
        #Check for a preprocessor directive
        if rawLine.find(preProcessorDirectives) >=0 :
            counts["preProcessor"] += 1
            continue


        #If currently in a multi line comment
        if skippingLines:
            counts["multiLine"] +=1
            #Check to see if it ends on the same line it opens
            endMultiLine = rawLine.find(multiLineCommentEnd)
            #Note: If a new multi line comment open on the same line one closes on, it will not find it
            if endMultiLine >=0:
                skippingLines = False
            continue


        #Note: If multi line starts on a line that contains code before, it will be marked as a comment
        locationIndex = rawLine.find(multiLineCommentStart)
        if(locationIndex >= 0):
            counts["multiLine"] +=1
            #If it does not end on same line, make as skipping lines
            endOnSameLine = rawLine.find(multiLineCommentEnd, locationIndex)
            if(endOnSameLine < 0):
                skippingLines = True
            continue


        locationIndex = rawLine.find(singleLineComment)
        #TODO: Add a counter for inline comments, must be stored different as it occupies same line of code
        #Store a bool of if this may be a single line, 
        # and substring everything before the comment
        if(locationIndex >=0):
            line = rawLine[:locationIndex]
            possSingleLine = True
        else:
            line = rawLine 
            possSingleLine = False;   
        
        #If line is only a space, mark it as comment/white space as appropriate to the flag
        if(line.isspace()):
            if possSingleLine:
                counts["singleLine"] +=1
            else:
                counts["whiteSpace"] +=1
            continue

        #Otherwise it must be code
        counts["code"] +=1
   #     output.write(line)        



   #sum everything that was counted
    sum =0
    for entry in counts:
        sum += counts[entry]
    #close the file
    file.close()

    #Return the formatted dictionary
    return {
        "name" : name,
        "total" : sum,
        "counts":counts
    }


#This function is responsible for checking a flat directory and counting all files
def checkDirectory(directory):

    #Init dictionary
    counts = {
        "preProcessor" : 0,
        "multiLine" : 0,
        "singleLine" : 0,
        "whiteSpace" : 0,
        "code" : 0,
        "total" :0
    }

    #Store all the file results
    allFiles = []
    for file in os.listdir(directory):
        #Check the file
        filename = os.fsdecode(file)
        result = parseFile(directory+"\\"+filename)

        #Sum the resulting dictionary and increment overarching total
        for field in result["counts"]:
            counts[field] += result["counts"][field]
        counts["total"] += result["total"]
        
        #Convert dictionary to list
        asArray = [filename]
        for element in result["counts"].values():
           asArray.append(element)
        asArray.append(result["total"])
        allFiles.append(asArray)    

    #Prepend the directory to the list of everything
    directoryTable = [directory] + list(counts.values())
    tableData = [directoryTable]
    for data in allFiles:
        tableData.append(data)

    #output the data    
    printTable(tableData) 
       

#This function is responsible for outputting the data, to console or file
def printTable(data):

    #Create the headers
    headers = ["Name", "PreProc", "MultiLine", "SingleLine", "whiteSpace", "Code", "Total"]

    #If not quiet, output the data
    if not args.quiet:
        print(tabulate(data, headers, "fancy_grid"))


    #If outputting to a file
    if(args.output):
        outputFile = open(args.output, "w")
        outputFile.write(tabulate(data, headers, "plain"))
        outputFile.close()




#Main
parser = argparse.ArgumentParser(description="Count lines of code in a directory or file")
parser.add_argument( 'input', metavar="<file or Directory>", help="The file or directory to check the lines of code")
parser.add_argument("-o", "--output", metavar="<filename>", help="Optionial file name to output data to")

group = parser.add_mutually_exclusive_group()
group.add_argument("-v", "--verbose", help="If each file should be outputted", action="store_true")
group.add_argument("-q", "--quiet", help="Surpress all console outputs", action="store_true")

args = parser.parse_args()


if(not os.path.exists(args.input)):
    print("File/Directory does not exist")
    exit(-1)

if os.path.isdir(args.input):
    checkDirectory(args.input)
elif os.path.isfile(args.input):
    parseFile(args.input)