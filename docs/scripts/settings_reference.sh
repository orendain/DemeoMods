#!/usr/bin/env bash

# Simple bash script to scrape the HouseRules SettingsReference.md doc from the Demeo Source Code.
# Usage: SOURCEDIR="./dnSpy Export v1.25/Assembly-CSharp" settings_reference.sh
# Usage 2: settings_refrence.sh "./dnSpy Export v1.25/Assembly-CSharp"

set -eo pipefail

if [ "$#" -eq 0 ]; then
	# Environment variable is required if we're runnign without arguments.
	if [ -z "${SOURCEDIR}" ]; then
		echo "SOURCEDIR variable must be set if not using positional arguments"
		echo "Usage: SOURCEDIR=<ASSembly-CSharp_Dir_Path> ./scripts/settings_reference.sh"
		echo "Example: SOURCEDIR='./dnSpy Export v1.25/Assembly-CSharp' settings_reference.sh"
		echo "or   : ./settings_refrence.sh './dnSpy Export v1.25/Assembly-CSharp'"
	        exit 1
	fi

elif [ "$#" -eq 1 ]; then
	SOURCEDIR="${1}"
fi

cat << 'EOF'
# Demeo Parameter Names

A full list of all of the different parameter names used by HouseRules rules

EOF

for item in AbilityKey Behaviour BoardPieceId EffectStateType PieceType 
do
	echo "## ${item}(s)"
	echo
	cat "${SOURCEDIR}/DataKeys/${item}.cs" | grep , | sort | awk '{print $1}' | sed 's/,//' | sed 's/^[[:space:]]*//' | sed 's/^/-\ /'

done

