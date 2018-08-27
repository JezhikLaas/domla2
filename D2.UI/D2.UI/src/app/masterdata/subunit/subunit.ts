import {ISubunit} from './isubunit';

export abstract class SubUnit implements ISubunit {
  public Id: string;
  public Title: string;
  public Version: number;
  public Number?: number;
  constructor(
  ) {}
}
