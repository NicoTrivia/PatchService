
export class MailTemplate {
    _id: number;
    _mailAcknowledge: string;
    _mailCompleted: string;
    
    constructor(data: any) {
        this._id = data.id;
        this._mailAcknowledge = data.mailAcknowledge;
        this._mailCompleted = data.mailCompleted;
    }

    public get id(): number {
        return this._id;
    }

    public set id(id: number) {
        this._id = id;
    }
    
    public get mailAcknowledge(): string {
        return this._mailAcknowledge;
    }

    public set mailAcknowledge(s: string) {
        this._mailAcknowledge = s;
    }

    public get mailCompleted(): string {
        return this._mailCompleted;
    }

    public set mailCompleted(s: string) {
        this._mailCompleted = s;
    }


    toJSON(): any {
        return { id: this._id, mailAcknowledge: this._mailAcknowledge, mailCompleted: this._mailCompleted};
    }
}