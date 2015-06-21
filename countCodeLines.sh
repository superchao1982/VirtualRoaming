cslines=0
for file in ./VirtualRoaming/Assets/Scripts/*.cs
do
	
	temp=`cat ${file} |wc -l`
	echo "${file} lines: ${temp}"
	let cslines=cslines+$temp
done
echo "all cs lines: ${cslines}"

javalines=0
for file in ./InAndroid/VirtualRoaming/src/com/xfy/virtualroaming/*.java
do
	temp=`cat ${file} |wc -l`
	echo "${file} lines: ${temp}"
	let javalines=javalines+$temp
done
echo "all java lines: ${javalines}"
let allLines=cslines+javalines;
echo "all lines: ${allLines}"