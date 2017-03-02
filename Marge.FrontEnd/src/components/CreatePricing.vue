<template>
    <form>
        <div class="form-group" v-bind:class="[{ 'has-danger': formErrors.targetPrice }]">
            <label for="targetPrice">Target Price</label>
            <input type="number" v-model="price.targetPrice" class="form-control" id="targetPrice" placeholder="Enter the target price"
                number>
                <div v-if="formErrors.targetPrice" class="form-control-feedback">{{formErrors.targetPrice}}</div>
        </div>

        <div class="form-group" v-bind:class="[{ 'has-danger': formErrors.price }]">
            <label for="cost">Cost</label>
            <input type="number" v-model="price.cost" class="form-control" id="cost" placeholder="Enter the cost" number>
            <div v-if="formErrors.cost" class="form-control-feedback">{{formErrors.cost}}</div>
        </div>

        <button type="submit" v-on:click.prevent="onSubmit" class="btn btn-primary">Create</button>
    </form>
</template>

<script>
    import Vue from 'vue';

    export default {
        data() {
            return {
                formErrors: {},
                price: {
                    targetPrice: null,
                    cost: null
                }
            }
        },
        methods: {
            validateForm() {
                const errors = {};
                if (!this.price.targetPrice) {
                    errors.targetPrice = 'Target price is required'
                }
                if (!this.price.cost) {
                    errors.cost = 'Cost is required'
                }
                this.formErrors = errors;
                return Object.keys(errors).length === 0;
            },
            onSubmit() {
                if (this.validateForm()) {
                    var self = this
                    Vue.http.post('http://localhost:49842/api/price', JSON.stringify(this.price))
                        .then((response) => {
                            self.price.targetPrice = null
                            self.price.cost = null
                        })
                }
            }
        }
    }

</script>