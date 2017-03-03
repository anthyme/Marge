<template>
  <div class="marge">
    <h1>{{ msg }}</h1>
    <h2>Create Price</h2>
    <create-pricing> </create-pricing>
    <h2>Pricing list</h2>
    <pricing-list> </pricing-list>
  </div>
</template>

<script>
import PricingList from './PricingList'
import CreatePricing from './CreatePricing'

export default {
  components: {
    PricingList,
    CreatePricing
  },
  name: 'hello',
  data () {
    return {
      msg: 'Welcome to Your Marge Application powered by Viseo'
    }
  }
}
</script>

<!-- Add "scoped" attribute to limit CSS to this component only -->
<style scoped>
h1, h2 {
  font-weight: normal;
}

ul {
  list-style-type: none;
  padding: 0;
}

li {
  display: inline-block;
  margin: 0 10px;
}

a {
  color: #42b983;
}
</style>
