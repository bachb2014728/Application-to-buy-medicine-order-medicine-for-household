import createApiClient from "./api.service.js";
class ManufacturerService {
    constructor(baseUrl = "http://localhost:5086/api/v1/manufacturers") {
        this.api = createApiClient(baseUrl);
        this.token = localStorage.getItem('token');
    }
    async getAll() {
        return (await this.api.get("/", {}));
    }
}
export default new ManufacturerService();