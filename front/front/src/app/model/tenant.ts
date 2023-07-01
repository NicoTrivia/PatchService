export class Tenant {
    private _code: string;
    private _name: string;
    private _email: string;
    private _level: string;
    private _active: boolean;
    private _creation_date: Date;
    private _expiration_date: Date;
    
    constructor(data: any) {
        this._code = data.code;
        this._name = data.name;
        this._email = data.email;
        if (data.level)
            this._level = data.level;
        else
            this._level = "Silver";
        this._active = data.active;
        this._creation_date = data.creation_date;
        this._expiration_date = data.expiration_date;
    }

    public get code(): string {
        return this._code;
    }

    public set code(code: string) {
        this._code = code;
    }

    public get name(): string {
        return this._name;
    }

    public set name(name: string) {
        this._name = name;
    }

    public get level(): string {
        return this._level;
    }

    public set level(level: string) {
        this._level = level;
    }

    public get email(): string {
        return this._email;
    }

    public set email(email: string) {
        this._email = email;
    }

    public get active(): boolean {
        return this._active;
    }

    public set active(b: boolean) {
        this._active = b;
    }

    public get creation_date(): Date {
        return this._creation_date;
    }

    public set creation_date(b: Date) {
        this._creation_date = b;
    }

    public get expiration_date(): Date {
        return this._expiration_date;
    }

    public set expiration_date(b: Date) {
        this._expiration_date = b;
    }

    toJSON(): any {
        return { code: this._code, name: this.name, email: this.email, level: this.level, active: this.active, 
            creation_date: this._creation_date, expiration_date: this._expiration_date};
    }
}
