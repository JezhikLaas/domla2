
export interface IAdministrationUnitRaws {
  Id: string;
  UserKey: string;
  Title: string;
  Edit: string;
  Version: number;
  YearOfConstruction?: {
    Year: string;
    Month: string;
  };
  Entrances?: {
    Id: string;
    Title: string;
    Address: {
      Country?: {
        Iso2: string;
        Iso3: string;
        Name: string;
      };
      City: string;
      Street: string;
      Number: string;
      PostalCode: string;
    }
    Edit: string;
    SubUnits?: string;
    AdministrationUnitId: string;
  }[];
  AdministrationUnitProperties?: {
    Title: string;
    Description: string;
    Value?: {
      Tag: string;
      Raw: string;
    },
    Version: number,
    Id: string;
  }[];
  SubUnits?: {
    Id: string;
    Title: string;
    Number: number;
    Version: number;
    Type?: number;
    Floor?: string;
    Entrance?: {
      Id: string;
      Title: string;
      Address: {
        Country?: {
          Iso2: string;
          Iso3: string;
          Name: string;
        };
        City: string;
        Street: string;
        Number: string;
        PostalCode: string;
      }
      Edit: string;
      SubUnits?: string;
      AdministrationUnitId: string;
    }
  }[];
  UnboundSubUnits?: {
    Id: string;
    Title: string;
    Number: number;
    Usage: string;
    Version: number;
    Type: string;
  }[];
}
