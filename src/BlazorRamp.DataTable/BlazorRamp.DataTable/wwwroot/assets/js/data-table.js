const setTempContentFocusModifier = (tableElement, contentElement, modifierClass) => {
    if (!tableElement || !contentElement)
        return;
    contentElement.classList.add(modifierClass);
    tableElement.addEventListener('focusout', () => contentElement.classList.remove(modifierClass), { once: true });
    tableElement.focus();
};
export { setTempContentFocusModifier };
//# sourceMappingURL=data-table.js.map