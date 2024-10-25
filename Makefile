compile:
	mcs src/ui/*.cs src/backend/*.cs vendor/*.cs -r:bin/FNA.dll -r:System.Windows.Forms.dll -r:System.Drawing.dll -win32icon:icon.ico -out:bin/main.exe
run: bin/main.exe
	mono bin/main.exe
	
compile_test:
	mcs src/backend/*.cs src/ui/ChartManager.cs vendor/*.cs test/*.cs -r:bin/FNA.dll -r:System.Windows.Forms.dll -r:System.Drawing.dll -out:bin/test.exe
test: compile_tests
	mono bin/test.exe
