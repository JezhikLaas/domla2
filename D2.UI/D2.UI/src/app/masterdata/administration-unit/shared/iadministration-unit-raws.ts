
export interface IAdministrationUnitRaws {
  Id: string;
  UserKey: string;
  Title: string;
  Edit: string;
  Version: number;
  YearOfConstruction?: {
    Year: number;
    Month: number;
  };
  Entrances?: {
    Id: string;
    Title: string;
    Address: {
      Country?: {
        Iso2: string;
        Name: string;
      };
      City: string;
      Street: string;
      Number: string;
      PostalCode: string;
    }
    Edit: string;
    Version: number;
    SubUnits?:
      {
        Floor: string,
        Title: string
        Number: number,
        Version: number,
        Id: string,
        Edit: string
      }[];
  }[];
  AdministrationUnitProperties?: {
    Title: string;
    Description: string;
    Value?: {
      Tag: number;
      Raw: string;
    },
    Version: number,
    Id: string;
    Edit: string;
  }[];
  SubUnits?: {
    Id: string;
    Title: string;
    Number: number;
    Version: number;
    Type?: number;
    Floor?: string;
    Edit: string;
    Entrance?: {
      Id: string;
      Title: string;
      Address: {
        Country?: {
          Iso2: string;
          Name: string;
        };
        City: string;
        Street: string;
        Number: string;
        PostalCode: string;
      }
      Edit: string;
      Version: number;
      SubUnits?:
        {
          Floor: string,
          Title: string
          Number: number,
          Version: number,
          Id: string,
          Edit: string
        }[];
    }
  }[];
  UnboundSubUnits?: {
    Id: string;
    Title: string;
    Number: number;
    Version: number;
    Type: number;
    Edit: string;
  }[];
}
