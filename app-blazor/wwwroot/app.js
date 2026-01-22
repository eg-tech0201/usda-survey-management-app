window.smsScrollToEvent = (container, target) => {
  if (!container || !target) {
    return;
  }

  const containerRect = container.getBoundingClientRect();
  const targetRect = target.getBoundingClientRect();
  const offsetTop = targetRect.top - containerRect.top + container.scrollTop;
  const nextScroll = offsetTop - (container.clientHeight / 2) + (targetRect.height / 2);

  container.scrollTop = Math.max(nextScroll, 0);
};

window.smsScrollToHour = (container, hour) => {
  if (!container) {
    return;
  }

  const row = container.querySelector(`[data-hour="${hour}"]`);
  if (!row) {
    return;
  }

  const containerRect = container.getBoundingClientRect();
  const rowRect = row.getBoundingClientRect();
  const offsetTop = rowRect.top - containerRect.top + container.scrollTop;

  container.scrollTop = Math.max(offsetTop - 16, 0);
};

window.smsPositionPopover = (eventEl, pop) => {
  if (!eventEl || !pop) {
    return;
  }

  const pad = 8;
  const eventRect = eventEl.getBoundingClientRect();

  pop.style.position = "fixed";
  pop.style.transform = "none";
  pop.style.left = `${eventRect.left}px`;
  pop.style.top = `${eventRect.bottom + 6}px`;

  const popRect = pop.getBoundingClientRect();
  let left = popRect.left;
  let top = popRect.top;

  if (popRect.right > window.innerWidth - pad) {
    left = window.innerWidth - pad - popRect.width;
  }
  if (popRect.left < pad) {
    left = pad;
  }
  if (popRect.bottom > window.innerHeight - pad) {
    top = eventRect.top - popRect.height - 6;
  }
  if (top < pad) {
    top = pad;
  }

  pop.style.left = `${left}px`;
  pop.style.top = `${top}px`;
};
