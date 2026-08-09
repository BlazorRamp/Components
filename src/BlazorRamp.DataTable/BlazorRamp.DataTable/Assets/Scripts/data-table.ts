
const setTempContentFocusModifier = (tableElement: HTMLElement, contentElement: HTMLElement, modifierClass: string): void => {

    if (!tableElement || !contentElement) return;

    contentElement.classList.add(modifierClass);

    tableElement.addEventListener('focusout', () => contentElement.classList.remove(modifierClass), { once: true});

    tableElement.focus();
};

export { setTempContentFocusModifier };