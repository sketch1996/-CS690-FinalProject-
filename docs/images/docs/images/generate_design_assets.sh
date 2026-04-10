#!/bin/bash
set -e

echo "Generating activity diagrams..."

cat > docs/images/activity-add-device.dot <<'DOT'
digraph {
  rankdir=TB;
  node [shape=box, fontname="Arial"];
  edge [fontname="Arial"];
  Start [shape=ellipse];
  End [shape=ellipse];

  Start -> "Open Add Device screen" -> "Enter device details" -> "System validates input" -> Decision;
  Decision [shape=diamond, label="Input valid?"];

  Decision -> "Save new device record" [label="Yes"];
  Decision -> "Show validation error" [label="No"];

  "Save new device record" -> "Show success message" -> End;
  "Show validation error" -> "Return to Add Device form" -> End;
}
DOT

cat > docs/images/activity-search-device.dot <<'DOT'
digraph {
  rankdir=TB;
  node [shape=box, fontname="Arial"];
  edge [fontname="Arial"];
  Start [shape=ellipse];
  End [shape=ellipse];

  Start -> "Open Search Device screen" -> "Enter device ID / name / IP" -> "System searches device records" -> Decision;
  Decision [shape=diamond, label="Device found?"];

  Decision -> "Display device details" [label="Yes"];
  Decision -> "Show 'Device not found'" [label="No"];

  "Display device details" -> End;
  "Show 'Device not found'" -> "Prompt: Search again or Add Device" -> End;
}
DOT

cat > docs/images/activity-update-status.dot <<'DOT'
digraph {
  rankdir=TB;
  node [shape=box, fontname="Arial"];
  edge [fontname="Arial"];
  Start [shape=ellipse];
  End [shape=ellipse];

  Start -> "Open Update Status screen" -> "Enter device search criteria" -> "System searches device records" -> Decision;
  Decision [shape=diamond, label="Device found?"];

  Decision -> "Select new status" [label="Yes"];
  Decision -> "Show 'Device not found'" [label="No"];

  "Select new status" -> "Save updated status" -> "Show confirmation" -> End;
  "Show 'Device not found'" -> "Return to Update Status screen" -> End;
}
DOT

cat > docs/images/activity-assign-technician.dot <<'DOT'
digraph {
  rankdir=TB;
  node [shape=box, fontname="Arial"];
  edge [fontname="Arial"];
  Start [shape=ellipse];
  End [shape=ellipse];

  Start -> "Open Assign Technician screen" -> "Enter device search criteria" -> "System searches device records" -> Decision;
  Decision [shape=diamond, label="Device found?"];

  Decision -> "Select technician" [label="Yes"];
  Decision -> "Show 'Device not found'" [label="No"];

  "Select technician" -> "Save technician assignment" -> "Show confirmation" -> End;
  "Show 'Device not found'" -> "Return to Assign Technician screen" -> End;
}
DOT

cat > docs/images/activity-log-issue.dot <<'DOT'
digraph {
  rankdir=TB;
  node [shape=box, fontname="Arial"];
  edge [fontname="Arial"];
  Start [shape=ellipse];
  End [shape=ellipse];

  Start -> "Open Log Issue screen" -> "Enter device search criteria" -> "System searches device records" -> Decision;
  Decision [shape=diamond, label="Device found?"];

  Decision -> "Enter issue details" [label="Yes"];
  Decision -> "Show 'Device not found'" [label="No"];

  "Enter issue details" -> "Save issue log" -> "Show confirmation" -> End;
  "Show 'Device not found'" -> "Return to Log Issue screen" -> End;
}
DOT

cat > docs/images/activity-generate-report.dot <<'DOT'
digraph {
  rankdir=TB;
  node [shape=box, fontname="Arial"];
  edge [fontname="Arial"];
  Start [shape=ellipse];
  End [shape=ellipse];

  Start -> "Open Generate Report screen" -> "Select report criteria" -> "System validates criteria" -> Decision;
  Decision [shape=diamond, label="Criteria valid?"];

  Decision -> "Generate report" [label="Yes"];
  Decision -> "Show validation error" [label="No"];

  "Generate report" -> "Display / export report" -> End;
  "Show validation error" -> "Return to Generate Report screen" -> End;
}
DOT

for f in docs/images/*.dot; do
  dot -Tpng "$f" -o "${f%.dot}.png"
done

echo "Diagrams created."
