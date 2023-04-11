var Animation = {}

Animation.Duration = function(duration, target) {
    let durationMs = duration*1000
    function lerp(a, b, t) {
        return a + t * (b - a)
    }
    if (this.startValue == undefined) {
        this.startTime = performance.now()
        this.startValue = target
        this.targetValue = target
        this.currentValue = target
        return target
    }


    if (target != this.targetValue) {
        this.startTime = performance.now()
        this.startValue = this.currentValue
        this.targetValue = target
    }

    let elapsed = performance.now() - this.startTime
    if (elapsed < durationMs) {
        window.AnimationFrame
    }
    let normalizedTime = Math.min(elapsed / durationMs, 1)

    this.currentValue = lerp(this.startValue, this.targetValue, normalizedTime)

    return this.currentValue
}

Animation.SpeedLimit = function (speedLimit, target) {

    if (this.currentValue == undefined) {
        this.currentValue = target
        return target
    }

    if (this.currentValue != target) {
        let now = performance.now()
        if (this.currentValueTime == undefined) {
            this.currentValueTime = now
        } else {
            let deltaTime = (now - this.currentValueTime) * 0.001
            this.currentValueTime = now
            let delta = target - this.currentValue

            let step = deltaTime * speedLimit
            if (Math.sign(delta) > 0) {
                this.currentValue = Math.min(this.currentValue + step, target)
            } else {
                this.currentValue = Math.max(this.currentValue - step, target)
            }
        }

        if (this.currentValue != target) {
            window.AnimationFrame
        } else {
            this.currentValueTime = undefined
        }
    }

    return this.currentValue
}