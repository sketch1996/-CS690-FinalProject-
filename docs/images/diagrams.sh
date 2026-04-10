#!/bin/bash

# SEARCH DEVICE
cat << 'DOT' > search.dot
digraph {
rankdir=TB;
node [shape=box];

Start [shape=ellipse];
End [shape=ellipse];

Start -> "Open Search Screen" -> "Enter Device Info" -> "Search Records" -> Decision;
Decision [shape=diamond label="Device Found?"];

Decision -> "Display Device Info" [label="Yes"];
Decision -> "Show Not Found" [label="No"];

"Display Device Info" -> End;
"Show Not Found" -> "Retry or Add Device" -> End;
}
DOT

# ADD DEVICE
cat << 'DOT' > add.dot
digraph {
rankdir=TB;
node [shape=box];

Start [shape=ellipse];
End [shape=ellipse];

Start -> "Open Add Device" -> "Enter Device Data" -> "Validate Input" -> Decision;
Decision [shape=diamond label="Valid Input?"];

Decision -> "Save Device" [label="Yes"];
Decision -> "Show Error" [label="No"];

"Save Device" -> End;
"Show Error" -> "Retry Input" -> End;
}
DOT

# UPDATE STATUS
cat << 'DOT' > update.dot
digraph {
rankdir=TB;
node [shape=box];

Start [shape=ellipse];
End [shape=ellipse];

Start -> "Search Device" -> Decision;
Decision [shape=diamond label="Device Found?"];

Decision -> "Update Status" [label="Yes"];
Decision -> "Show Not Found" [label="No"];

"Update Status" -> "Save Status" -> End;
"Show Not Found" -> End;
}
DOT

# ASSIGN TECH
cat << 'DOT' > assign.dot
digraph {
rankdir=TB;
node [shape=box];

Start [shape=ellipse];
End [shape=ellipse];

Start -> "Search Device" -> Decision;
Decision [shape=diamond label="Device Found?"];

Decision -> "Assign Technician" [label="Yes"];
Decision -> "Show Not Found" [label="No"];

"Assign Technician" -> "Save Assignment" -> End;
"Show Not Found" -> End;
}
DOT

# LOG ISSUE
cat << 'DOT' > issue.dot
digraph {
rankdir=TB;
node [shape=box];

Start [shape=ellipse];
End [shape=ellipse];

Start -> "Search Device" -> Decision;
Decision [shape=diamond label="Device Found?"];

Decision -> "Log Issue" [label="Yes"];
Decision -> "Show Not Found" [label="No"];

"Log Issue" -> "Save Issue" -> End;
"Show Not Found" -> End;
}
DOT

# GENERATE IMAGES
dot -Tpng search.dot -o activity-search.png
dot -Tpng add.dot -o activity-add.png
dot -Tpng update.dot -o activity-update.png
dot -Tpng assign.dot -o activity-assign.png
dot -Tpng issue.dot -o activity-issue.png

echo "Diagrams created in docs/images/"
