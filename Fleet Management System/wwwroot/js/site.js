window.downloadTextFile = (filename, content, mimeType) => {
    const blob = new Blob([content], { type: mimeType || 'text/plain;charset=utf-8;' });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(url);
};

window.printPage = () => window.print();
