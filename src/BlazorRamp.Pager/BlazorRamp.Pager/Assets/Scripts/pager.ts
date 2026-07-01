const setSelectorFocus = (elementId: string): void => {

    const element: HTMLElement = document.getElementById(elementId) as HTMLElement;

    console.log(`ID: ${elementId}\nValue: ${element}\n`);

    if (!element) return;

    element.focus();
};


export { setSelectorFocus };