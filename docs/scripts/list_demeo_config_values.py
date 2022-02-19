#!/usr/bin/env python3

"""
Uses regular expression to find all of the Xxx.Xxx fields within GameDataAPI.cs and arrange them for pretty printing.
This provides a convenient way of getting a list of all AbilityKey, Behaviour, BoardPieceId, EffectStateType & PieceType
avaiable for use in HouseRules rulesets. 
"""

import sys
import re

if len(sys.argv)<2:
    print(f"{sys.argv[0]} - Generates Markdown document listing Demeo parameter names")
    print(f"Usage: {sys.argv[0]} <path to GameDataApi.cs file>", file=sys.stderr)
    sys.exit(1)
GameDataAPIFile=sys.argv[1]
GameData=open(GameDataAPIFile,'r').readlines()
results={}
for line in GameData:
    x = re.search("[A-Z]\w+\.[A-Z]\w+", line)
    if x is not None:
        key, value = x.group().split('.')
        if key not in results:
            results[key]=[value]
        elif value not in results[key]:
            results[key].append(value)

print(f"# Demeo Parameter Names\n")
print(f"A full list of all of the different parameter names used by HouseRules rules\n")

for key in ["AbilityKey", "Behaviour", "BoardPieceId", "EffectStateType", "PieceType" ]:
    print(f"## {key}(s)\n")
    for value in sorted(results[key]):
        print(f"- {value}")
    print()

