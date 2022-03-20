#!/usr/bin/env python3

"""
Uses regular expression to find all of the properties within DreadLevelsDTO.cs and arrange them for pretty printing.
"""

import sys
import re

if len(sys.argv)<2:
    print(f"{sys.argv[0]} - Generates Markdown document listing Demeo level properties")
    print(f"Usage: {sys.argv[0]} <path to DreadLevelsDTO.cs file>", file=sys.stderr)
    sys.exit(1)

file=sys.argv[1]
fileLines=open(file,'r').readlines()
results={}
for line in fileLines:
    x = re.search("(public|private|internal)\s(.*)\s([A-Za-z]*);", line)
    if x is not None:
        results[x.group(3)]=x.group(2)

print(f"# Demeo Level Properties\n")
print(f"A list of all level properties defined by Demeo, and their types.\n")
print(f"> Note: There is no guarantee that setting a particular field will result in the changes that you might expect. Demeo ignores many of these properties. It is out of scope of this documentation to list what can be done with the properties below.\n")

for key, value in results.items():
    print(f"- {key} ({value})")
print()
