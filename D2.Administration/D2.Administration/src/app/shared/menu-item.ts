export class MenuItem {
  constructor(
    public label: string,
    public onClick: () => void,
    public isActive: () => boolean
  ) {}
}
