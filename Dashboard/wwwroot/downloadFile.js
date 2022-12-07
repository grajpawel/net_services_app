window.downloadFileFromStream = async (fileName, streamReference) => {
    const arrayBuffer = await streamReference.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);
    const anchorElemenent = document.createElement('a');
    anchorElemenent.href = url;
    anchorElemenent.download = fileName ?? '';
    anchorElemenent.click();
    anchorElemenent.remove();
    URL.revokeObjectURL(url);
}