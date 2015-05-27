cd "E:\project\VirtualRoaming\InAndroid\VirtualRoaming\bin\classes"
e:
jar -cvf c.jar *
copy c.jar "E:\project\VirtualRoaming\VirtualRoaming\Assets\Plugins\Android\bin"
cd ../..
copy AndroidManifest.xml "E:\project\VirtualRoaming\VirtualRoaming\Assets\Plugins\Android"
pause