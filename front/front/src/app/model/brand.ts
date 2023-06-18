
export class Brand {
    _name: string;
    _code: string;

    constructor(data: any) {
        this._code = data.code;
        this._name = data.name;
    }
    
    public get code(): string {
        return this._code;
    }

    public set code(s: string) {
        this._code = s;
    }

    public get name(): string {
        return this._name;
    }

    public set name(name: string) {
        this._name = name;
    }
}