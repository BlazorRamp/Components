const getScreenInnerWidth = (): number => window.innerWidth;

function testModuleIsWorking(message: string): void {
    alert('Hello from javascript');
}

export { getScreenInnerWidth, testModuleIsWorking };