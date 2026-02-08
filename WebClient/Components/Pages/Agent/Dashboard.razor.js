export class Dashboard {
    
}
export function downloadFileFromStream(fileName, content) {

    const blob = new Blob([content], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    const url = window.URL.createObjectURL(blob);

    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = fileName ?? 'report.xlsx';
    anchorElement.click();

    anchorElement.remove();
    window.URL.revokeObjectURL(url);
}
window.Dashboard = Dashboard;