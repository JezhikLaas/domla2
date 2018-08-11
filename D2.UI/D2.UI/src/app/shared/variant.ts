export class Variant {
  public Tag = 3;
  public Raw?: any;

  constructor(tag: number, raw: any) {
    this.Tag = tag;
    switch (tag) {
      case 1:
        this.Raw = raw;
        break;
      case 3:
        this.Raw = raw;
        break;
      case 2:
        this.Raw = new TypedValue(raw._value, raw._unit, raw._decimalPlaces);
    }
  }
}
export class TypedValue {
  public _value: number;
  public _decimalPlaces: number;
  public _unit: string;

  constructor (value: number, unit: string, decimalPlaces: number ) {
    this._value = value;
    this._decimalPlaces = decimalPlaces;
    this._unit = unit;
  }
}
